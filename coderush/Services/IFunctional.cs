using coderush.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace coderush.Services
{
    public interface IFunctional
    {
        Task InitAppData();

        Task CreateDefaultSuperAdmin();

        Task SendEmailBySendGridAsync(string apiKey, 
            string fromEmail, 
            string fromFullName, 
            string subject, 
            string message, 
            string email);

        Task SendEmailByGmailAsync(string fromEmail,
            string fromFullName,
            string subject,
            string messageBody,
            string toEmail,
            string toFullName,
            string smtpUser,
            string smtpPassword,
            string smtpHost,
            int smtpPort,
            bool smtpSSL);

        Task<string> UploadFile(List<IFormFile> files, IHostingEnvironment env, string uploadFolder);

        string GetCurrentLoginUserId();

        void AddAuditInfo<TEntity>(ref TEntity entity);

        void UpdateAuditInfo<TEntity>(ref TEntity entity);

        ResultModel Insert<TEntity>(TEntity entity) where TEntity : class;

        ResultModel Update<TEntity>(TEntity entity) where TEntity : class;

        ResultModel Update<TSource, TDestination>(TSource entity, object id) where TSource : class where TDestination : class;

        ResultModel Delete<TEntity>(object id) where TEntity : class;

        ResultModel Delete<TEntity>(TEntity entity) where TEntity : class;

        TEntity GetById<TEntity>(object id) where TEntity : class;

        IEnumerable<TEntity> GetList<TEntity>(Expression<Func<TEntity, bool>> predicate) where TEntity : class;

        IEnumerable<TEntity> GetList<TEntity>() where TEntity : class;

    }
}
