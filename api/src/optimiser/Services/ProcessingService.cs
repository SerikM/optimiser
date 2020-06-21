﻿using Optimiser.Models;
using System;
using System.Linq;
using System.Globalization;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Extensions.Logging;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.InteropServices;

namespace Optimiser.Services
{
    public class ProcessingService : IProcessingService
    {
        private readonly IDBDataService<IData> _dataService;
        private const int MaxNumberOfCommercialsPerBrake = 3;
        private const int MaxNumberOfCommercialsPerBrakeWithSameType = 2;


        public ProcessingService(IDBDataService<IData> dataService)
        {
            _dataService = dataService;
        }

        public async Task<object> GetData()
        {
            //var breaks = await _dataService.GetItems<Break>();
            //var commercials = await _dataService.GetItems<Commercial>();

            var breaks = _dataService.GetBreaks();
            var commercials = _dataService.GetCommercials();
            int start = 0;
            foreach (var comm in commercials) 
            {
                var newComm = (Commercial)comm;

                ProcessRecursively(breaks, newComm, start);


            }

            return null;
        }


        public void ProcessRecursively(List<Break> breaks, Commercial currComm,int start) 
        {
            Break bestBr = null;
            var breakIntern = breaks.Skip(start).ToList();
            foreach (var br in breakIntern)
            {
                if (bestBr == null) { bestBr = br; continue; }
                var newScore = br.Ratings.FirstOrDefault(p => p.DemoType == currComm.TargetDemo).Score;
                var currentScore = bestBr.Ratings.FirstOrDefault(p => p.DemoType == currComm.TargetDemo).Score;
                if (newScore > currentScore) bestBr = br;
            }

            if (bestBr.Commercials == null || !bestBr.Commercials.Any())
            {
                currComm.CurrentRating = bestBr.Ratings.FirstOrDefault(p => p.DemoType == currComm.TargetDemo);
                bestBr.Commercials = new List<Commercial>() { currComm };
            }
            else if (bestBr.Commercials.Count < MaxNumberOfCommercialsPerBrake && IsAllowedToAdd(bestBr, currComm))
            {
                currComm.CurrentRating = bestBr.Ratings.FirstOrDefault(p => p.DemoType == currComm.TargetDemo);
                bestBr.Commercials.Add(currComm);
            }
            else
            {
                if (ShouldBeReplaced(currComm, bestBr))
                {
                    var newScore = GetScore(bestBr, currComm);
                    bestBr.Commercials = bestBr.Commercials.OrderByDescending(d => d.CurrentRating.Score).ToList();

                    var commToBeShifted = bestBr.Commercials.FirstOrDefault(d => d.CurrentRating.Score < newScore);

                    currComm.CurrentRating = bestBr.Ratings.FirstOrDefault(d => d.DemoType == currComm.TargetDemo);
                    bestBr.Commercials.Insert(bestBr.Commercials.IndexOf(commToBeShifted), currComm);

                    var detachedCom = bestBr.Commercials.Last();
                    bestBr.Commercials.Remove(detachedCom);
                   
                    MoveItem(breaks.IndexOf(bestBr), start, breaks); 
                    ProcessRecursively(breaks, detachedCom, ++start);
                }
                else 
                {
                    MoveItem(breaks.IndexOf(bestBr), start, breaks);
                    ProcessRecursively(breaks, currComm, ++start);
                }
            }
        }

        private void MoveItem(int oldIndex, int newIndex, List<Break> items)
        {
            var target = items.ElementAt(oldIndex);
            items.RemoveAt(oldIndex);
            items.Insert(newIndex, target);
        }

        private bool IsAllowedToAdd(Break bestBr, Commercial currComm)
        {
            return true;
        }

        private bool ShouldBeReplaced(Commercial currComm, Break optmBr)
        {
            var newScore = GetScore(optmBr, currComm);
           
            if (optmBr.Commercials.Count(p => p.CommercialType == currComm.CommercialType) == MaxNumberOfCommercialsPerBrakeWithSameType && 
                !optmBr.Commercials.Any( d => d.CommercialType == currComm.CommercialType && d.CurrentRating.Score < newScore)) return false;
            return optmBr.Commercials.Any(p => p.CurrentRating.Score < newScore);
        }


        private int GetScore(Break br, Commercial comm) 
        {
           return br.Ratings.FirstOrDefault(p => p.DemoType == comm.TargetDemo).Score;
        }
    }
}
