using System.Linq;
using System.Collections.Generic;
using Optimiser.Models;

namespace Optimiser.Services
{
    public class ProcessingService : IProcessingService
    {
        private readonly IDbDataService<IData> _dataService;
        private const int MaxNumberOfCommercialsPerBrake = 3;
        private const int MaxNumberOfCommercialsPerBrakeWithSameType = 2;

        public ProcessingService(IDbDataService<IData> dataService)
        {
            _dataService = dataService;
        }

        public List<Break> GetDefaultData()
        {
            return _dataService.GetBreaksWithDefaultRatings();
        }

        public List<Break> GetData(List<Break> breaks)
        {
            var commercials = _dataService.GetCommercials();

            int start = 0;
            foreach (var comm in commercials)
            {
                ProcessRecursively(breaks, comm, start);
            }
            return OrderCommercials(breaks.OrderBy(p => p.Id).ToList());
        }

        private List<Break> OrderCommercials(List<Break> breaks)
        {
            breaks.ForEach((br) =>
            {
                OrderCommercials(br.Commercials);
            });
            return breaks;
        }

        private void OrderCommercials(IList<Commercial> commercials)
        {
            var rep = commercials.FirstOrDefault(p => 
                                commercials.IndexOf(p) + 1 < commercials.Count
                                && p.CommercialType == commercials.ElementAt(commercials.IndexOf(p) + 1).CommercialType);

            if (rep == null) return;

            var nonRep = commercials.FirstOrDefault(p => 
                                   p.CommercialType != rep.CommercialType
                                   && commercials.IndexOf(p) + 1 == commercials.Count);
            if (nonRep != null)
            {
                var tmp = rep;
                commercials.Remove(rep);
                commercials.Add(tmp);
                OrderCommercials(commercials);
                return;
            }

            nonRep = commercials.FirstOrDefault(p => 
                     p.CommercialType != rep.CommercialType
                     && commercials.IndexOf(p) + 1 < commercials.Count
                     && rep.CommercialType != commercials.ElementAt(commercials.IndexOf(p) + 1).CommercialType);

            if (nonRep != null)
            {
                var tmp = rep;
                commercials.Remove(rep);
                commercials.Insert(commercials.IndexOf(nonRep) + 1, tmp);
                OrderCommercials(commercials);
                return;
            }

            nonRep = commercials.FirstOrDefault(p => 
                     p.CommercialType != rep.CommercialType
                     && commercials.IndexOf(p) == 0);

            if (nonRep != null)
            {
                var tmp = rep;
                commercials.Remove(rep);
                commercials.Insert(0, tmp);
                OrderCommercials(commercials);
                return;
            }

            nonRep = commercials.FirstOrDefault(p => 
                     p.CommercialType != rep.CommercialType
                     && commercials.IndexOf(p) - 1 >= 0 && rep.CommercialType !=
                     commercials.ElementAt(commercials.IndexOf(p) - 1).CommercialType);

            if (nonRep != null)
            {
                var tmp = rep;
                commercials.Remove(rep);
                commercials.Insert(commercials.IndexOf(nonRep), tmp);
                OrderCommercials(commercials);
            }
        }

        private void ProcessRecursively(List<Break> breaks, Commercial currComm, int start)
        {
            Break bestBr = null;
            if (start == breaks.Count) return;

            var breakIntern = breaks.Skip(start).ToList();
            foreach (var br in breakIntern)
            {
                if (bestBr == null) { bestBr = br; continue; }
                var newScore = br.Ratings?.FirstOrDefault(p => p.DemoType == currComm.TargetDemo)?.Score;
                var currentScore = bestBr.Ratings?.FirstOrDefault(p => p.DemoType == currComm.TargetDemo)?.Score;
                if (newScore > currentScore) bestBr = br;
            }

            if (bestBr != null && (bestBr.Commercials == null || !bestBr.Commercials.Any()))
            {
                currComm.CurrentRating = bestBr.Ratings.FirstOrDefault(p => p.DemoType == currComm.TargetDemo);
                bestBr.Commercials = new List<Commercial>() { currComm };
            }
            else if (bestBr.Commercials.Count < MaxNumberOfCommercialsPerBrake && IsTypeAllowedInBrake(bestBr, currComm))
            {
                var newScore = GetScore(bestBr, currComm);
                if (bestBr.Commercials.Count(p => p.CommercialType == currComm.CommercialType) < MaxNumberOfCommercialsPerBrakeWithSameType)
                {
                    currComm.CurrentRating = bestBr.Ratings.FirstOrDefault(p => p.DemoType == currComm.TargetDemo);
                    bestBr.Commercials.Add(currComm);
                }
                else
                {
                    var toBeMoved = bestBr.Commercials.FirstOrDefault(d => d.CommercialType == currComm.CommercialType && d.CurrentRating.Score < newScore);
                    if (toBeMoved != null)
                    {
                        currComm.CurrentRating = bestBr.Ratings.FirstOrDefault(d => d.DemoType == currComm.TargetDemo);
                        bestBr.Commercials.Insert(bestBr.Commercials.IndexOf(toBeMoved), currComm);
                        bestBr.Commercials.Remove(toBeMoved);

                        MoveItem(breaks.IndexOf(bestBr), start, breaks);
                        ProcessRecursively(breaks, toBeMoved, ++start);
                    }
                    else
                    {
                        MoveItem(breaks.IndexOf(bestBr), start, breaks);
                        ProcessRecursively(breaks, currComm, ++start);
                    }
                }
            }
            else
            {
                if (ShouldBeInserted(currComm, bestBr) && IsTypeAllowedInBrake(bestBr, currComm))
                {
                    var newScore = GetScore(bestBr, currComm);
                    bestBr.Commercials = bestBr.Commercials.OrderByDescending(d => d.CurrentRating.Score).ToList();

                    var toBeMoved = bestBr.Commercials.FirstOrDefault(d => d.CurrentRating.Score < newScore);

                    currComm.CurrentRating = bestBr.Ratings.FirstOrDefault(d => d.DemoType == currComm.TargetDemo);
                    bestBr.Commercials.Insert(bestBr.Commercials.IndexOf(toBeMoved), currComm);

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

        private bool IsTypeAllowedInBrake(Break bestBr, Commercial currComm)
        {
            return bestBr.DisallowedCommTypes == null || bestBr.DisallowedCommTypes.All(d => d != currComm.CommercialType);
        }

        private bool ShouldBeInserted(Commercial currComm, Break optmBr)
        {
            var newScore = GetScore(optmBr, currComm);
            if (optmBr.Commercials.Count(p => p.CommercialType == currComm.CommercialType) == MaxNumberOfCommercialsPerBrakeWithSameType &&
                !optmBr.Commercials.Any(d => d.CommercialType == currComm.CommercialType && d.CurrentRating.Score < newScore)) return false;
            return optmBr.Commercials.Any(p => p.CurrentRating.Score < newScore);
        }

        private int GetScore(Break br, Commercial comm)
        {
            return br?.Ratings?.FirstOrDefault(p => p.DemoType == comm.TargetDemo)?.Score ?? 0;
        }
    }
}
