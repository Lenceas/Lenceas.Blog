﻿using Lenceas.Core.Model;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Lenceas.Core.IServices
{
    public interface IBaseServices<T> where T : class, new()
    {
        #region 是否存在
        Task<bool> IsExist(int id);
        Task<bool> IsExist(Expression<Func<T, bool>> whereLambda);
        #endregion

        #region 分页查询
        Task<List<T>> GetPage(int pageIndex, int pageSize);
        Task<List<T>> GetPage(int pageIndex, int pageSize, Expression<Func<T, bool>> whereLambda);
        #endregion

        #region 查询
        Task<T> GetById(int id);
        Task<List<T>> GetByIds(List<int> ids);
        Task<T> GetEntity(Expression<Func<T, bool>> whereLambda);
        Task<List<T>> GetList();
        Task<List<T>> GetList(Expression<Func<T, bool>> whereLambda);
        #endregion

        #region 增加
        Task<int> AddAsync(T entity);
        Task<int> AddBulkAsync(List<T> entities);
        #endregion

        #region 修改
        Task<int> UpdateAsync(Expression<Func<T, bool>> whereLambda, Expression<Func<T, T>> entity);
        Task<int> UpdateBulkAsync(List<T> entities);
        #endregion

        #region 删除
        Task<int> DeleteById(int id);
        Task<int> DeleteByIds(List<int> ids);
        Task<int> DeleteAsync(T entity);
        Task<int> DeletesAsync(List<T> entities);
        Task<int> DeleteAsync(Expression<Func<T, bool>> whereLambda);
        #endregion
    }
}
