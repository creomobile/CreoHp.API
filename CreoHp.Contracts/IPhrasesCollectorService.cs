using System.Threading.Tasks;

namespace CreoHp.Contracts
{
    public interface IPhrasesCollectorService
    {
        Task<string[]> Collect(string keyPhrase, int max);
    }
}
