//=====================================================================================
// Developed by Kallebe Lins (kallebe.santos@outlook.com)
// Teacher, Architect, Consultant and Project Leader
// Virtual Card: https://www.linkedin.com/in/kallebelins
//=====================================================================================
// Reproduction or sharing is free!
//=====================================================================================
using Mvp24Hours.Core.Contract.Logic.DTO;
using Mvp24Hours.Core.DTO.Logic;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Mvp24Hours.Application.Factory
{
    public class PagingResultFactory<T>
    {
        #region [ Tasks ]

        public static Task<IPagingResult<T>> TaskCreate(IList<T> _data)
        {
            return TaskCreate(_data, null, null);
        }

        public static Task<IPagingResult<T>> TaskCreate(IList<T> _data, IPageResult page, ISummaryResult summary)
        {
            return Task.FromResult(Create(_data, page, summary));
        }

        public static Task<IPagingResult<T>> TaskMapper<U>(IList<U> _data)
        {
            return TaskMapper(_data, null, null);
        }

        public static Task<IPagingResult<T>> TaskMapper<U>(IList<U> _data, IPageResult page, ISummaryResult summary)
        {
            return Task.FromResult(Mapper(_data, page, summary));
        }

        #endregion

        #region [ Auxiliars ]

        public static void InitializeMapper<R>()
        {
            var method = typeof(R).GetMethod("ConfigureMapper");
            if (method != null)
            {
                method.Invoke(null, null);
            }
        }

        public static IPagingResult<T> Create(IList<T> _data)
        {
            return Create(_data, null, null);
        }

        public static IPagingResult<T> Create(IList<T> _data, IPageResult page, ISummaryResult summary)
        {
            var bo = new PagingResult<T>(_data);
            bo.Paging = page;
            bo.Summary = summary;
            return bo;
        }

        public static IPagingResult<T> Mapper<U>(IList<U> _data)
        {
            return Mapper(_data, null, null);
        }

        public static IPagingResult<T> Mapper<U>(IList<U> _data, IPageResult page, ISummaryResult summary)
        {
            var bo = new PagingResult<T>();
            bo.Paging = page;
            bo.Summary = summary;
            InitializeMapper<T>();
            // AutoMapper.Mapper.Map(_data, bo.Data);
            return bo;
        }

        //public PagingResult<T> Clone<U>(IList<U> _data)
        //{
        //    this.data = (IList<T>)_data;
        //    return this;
        //}

        #endregion
    }
}
