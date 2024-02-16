using SegundaPracticaFundamentos.Models;

namespace SegundaPracticaFundamentos.Repositories
{
    public interface IRepositoryComics
    {
        List<Comic> GetComics();
        Comic GetComic(int idcomic);
        void InsertComicLambda(Comic comic);
        void InsertComicProcedure(Comic comic);
        void DeleteComic(int idcomic);
    }
}
