using System.Linq;
using System.Collections.Generic;
using Optimiser.Models;
using System.Threading.Tasks;

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

        /// <summary>
        /// retrieves default breaks data from the datastore
        /// </summary>
        /// <returns></returns>
        public List<Break> GetDefaultData()
        {
            return  _dataService.GetItems<Break>().Result?.OrderBy(d => d.Id).ToList();
        }

        /// <summary>
        /// serves as the main trigger to start the sorting 
        /// and ordering of commercials into optimal position
        /// </summary>
        /// <param name="breaks"></param>
        /// <returns></returns>
        public async Task<List<Break>> GetOptimalRatings(List<Break> breaks)
        {
            var commercials = await _dataService.GetItems<Commercial>();

            int start = 0;
            foreach (var comm in commercials)
            {
                ProcessRecursively(breaks, comm, start);
            }
            return OrderCommercials(breaks.OrderBy(p => p.Id).ToList());
        }

        /// <summary>
        /// initiates reordering of commercials within each brake in a list of brakes
        /// </summary>
        /// <param name="breaks"></param>
        /// <returns>finalised list of brakes</returns>
        public List<Break> OrderCommercials(List<Break> breaks)
        {
            breaks.ForEach((br) =>
            {
                OrderCommercials(br.Commercials);
            });
            return breaks;
        }

        /// <summary>
        /// iterates a list of commercials and reorders commercials in 
        /// such way that no commercials of the same type are placed next to each other within a brake
        /// </summary>
        /// <param name="commercials"></param>
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

        /// <summary>
        /// recursively iterates over the given collection of breaks and finds the best allocation 
        /// for a given commercial respecting given restrictions
        /// </summary>
        /// <param name="breaks"></param>
        /// <param name="currComm"></param>
        /// <param name="start"></param>
        private void ProcessRecursively(List<Break> breaks, Commercial currComm, int start)
        {
            if (start == breaks.Count) return;

            var breakIntern = breaks.Skip(start).ToList();
            Break bestBr = GetBestBreak(breakIntern, currComm);

            if (bestBr != null && (bestBr.Commercials == null || !bestBr.Commercials.Any()) && IsTypeAllowedInBrake(bestBr, currComm))
            {
                currComm.CurrentRating = bestBr.Ratings.FirstOrDefault(p => p.DemoType == currComm.TargetDemo);
                bestBr.Commercials = new List<Commercial>() { currComm };
            }
            else if (bestBr.Commercials!= null && bestBr.Commercials.Count < MaxNumberOfCommercialsPerBrake && IsTypeAllowedInBrake(bestBr, currComm))
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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="breakIntern"></param>
        /// <param name="currComm"></param>
        /// <returns></returns>
        private Break GetBestBreak(List<Break> breakIntern, Commercial currComm)
        {
            Break bestBr = null;
            foreach (var br in breakIntern)
            {
                if (bestBr == null) { bestBr = br; continue; }
                var newScore = br.Ratings?.FirstOrDefault(p => p.DemoType == currComm.TargetDemo)?.Score;
                var currentScore = bestBr.Ratings?.FirstOrDefault(p => p.DemoType == currComm.TargetDemo)?.Score;
                if (newScore > currentScore) bestBr = br;
            }
            return bestBr;
        }

        /// <summary>
        /// moves an item within a list
        /// </summary>
        /// <param name="oldIndex"></param>
        /// <param name="newIndex"></param>
        /// <param name="items"></param>
        private void MoveItem(int oldIndex, int newIndex, List<Break> items)
        {
            var target = items.ElementAt(oldIndex);
            items.RemoveAt(oldIndex);
            items.Insert(newIndex, target);
        }

        /// <summary>
        /// checks if a commercial can be added to the brake
        /// </summary>
        /// <param name="bestBr"></param>
        /// <param name="currComm"></param>
        /// <returns></returns>
        private bool IsTypeAllowedInBrake(Break bestBr, Commercial currComm)
        {
            return bestBr.DisallowedCommTypes == null || bestBr.DisallowedCommTypes.All(d => d != currComm.CommercialType);
        }

        /// <summary>
        /// checks if the gived commercial has a rating higher than other commercials 
        /// within a specific break
        /// </summary>
        /// <param name="currComm"></param>
        /// <param name="optmBr"></param>
        /// <returns></returns>
        private bool ShouldBeInserted(Commercial currComm, Break optmBr)
        {
            var newScore = GetScore(optmBr, currComm);
            if (optmBr.Commercials.Count(p => p.CommercialType == currComm.CommercialType) == MaxNumberOfCommercialsPerBrakeWithSameType &&
                !optmBr.Commercials.Any(d => d.CommercialType == currComm.CommercialType && d.CurrentRating.Score < newScore)) return false;
            return optmBr.Commercials.Any(p => p.CurrentRating.Score < newScore);
        }

        /// <summary>
        /// retrieves the score a brake has for a particular commercial
        /// </summary>
        /// <param name="br"></param>
        /// <param name="comm"></param>
        /// <returns></returns>
        private int GetScore(Break br, Commercial comm)
        {
            return br?.Ratings?.FirstOrDefault(p => p.DemoType == comm.TargetDemo)?.Score ?? 0;
        }

        /// <summary>
        /// can be trigger from a controller or another service in order
        /// to seed brakes and commercials in the DynamoDb
        /// </summary>
        /// <returns>true if data seed is successful</returns>
        public bool SeedData() 
        {
          return  _dataService.SeedItems();
        }
    }
}
