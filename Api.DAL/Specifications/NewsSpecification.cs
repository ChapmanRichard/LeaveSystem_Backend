using Api.Common.Specifications;
using Api.Contract.Model;
using System;

namespace Api.DAL.Specifications
{
    public class NewsSpecification
    {

        public static Specification<News> Base => new Specification<News>(x => x.NewsId > 0);
        public static Specification<News> Id(int query) => new Specification<News>(x => x.NewsId == query);

        public static Specification<News> NewsDate(DateTime query) => new Specification<News>(x => x.NewsDate.Date == query.Date);

        public static Specification<News> Content(string query) => new Specification<News>(x => x.Content.Contains(query));
        public static Specification<News> Status(bool query) => new Specification<News>(x => x.Status == query);

    }
}
