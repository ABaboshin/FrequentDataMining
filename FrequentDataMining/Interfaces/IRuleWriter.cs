// MIT License.
// (c) 2015, Andrey Baboshin

using FrequentDataMining.AgrawalFaster;

namespace FrequentDataMining.Interfaces
{
    /// <summary>
    /// rule writer
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IRuleWriter<T>
    {
        /// <summary>
        /// writer a rule
        /// </summary>
        /// <param name="rule"></param>
        void SaveRule(Rule<T> rule);
    }
}
