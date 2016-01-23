using FlowRepository.Repositories.Contracts.FlowRepository;
using FlowRepository.Repositories.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlowRepository.Repositories.Models.FlowLog
{
    public class LogRepository : DataRepository<FlowCollectionLogEntities>, IFlowLogRepository
    {
        /// <summary>
        /// Logs an exception thrown from the client and stores information based on configuration
        /// </summary>
        /// <param name="exception"> Exception thrown</param>
        /// <param name="storeStackTrace">weather you should store the stack trace</param>
        /// <param name="isRecursive">if you should recursively store stack traces</param>
        public void AddError(Exception exception, string url = null, bool storeStackTrace = false)
        {
            var error = GetErrorFromException(exception, storeStackTrace);

            //Only Store the url information once on root
            error.UrlRoute = url;

            _context.Errors.Add(error);

            SaveChanges();
        }

        public void AddInfo(string message, string infoTypeName)
        {
            var infoType = GetOrAddInfoTypeByName(infoTypeName);
            var info = new Info
            {
                Message = message,
                InfoType = infoType,
                Timestamp = DateTime.Now,
            };

            _context.Infoes.Add(info);
        }

        protected void AddErrorType(string name)
        {
            var errorType = new ErrorType { Name = name };
            Context.ErrorTypes.Add(errorType);
        }

        protected Error GetErrorFromException(Exception exception, bool storeStackTrace)
        {
            var error = new Error();

            error.ErrorMessage = exception.Message;
            error.Timestamp = DateTime.Now;

            if(storeStackTrace)
            {
                error.StackTrace = exception.StackTrace;
            }

            error.ErrorType = GetErrorType(exception);

            if(null != exception.InnerException)
            {
                error.Errors1.Add(GetErrorFromException(exception.InnerException, storeStackTrace));
            }

            return error;
        }

        protected ErrorType GetErrorType(Exception exception)
        {
            var errorTypeName = exception.GetType().Name;
            var errorType = _context.ErrorTypes.FirstOrDefault(e => e.Name == errorTypeName);

            if(null == errorType)
            {
                AddErrorType(errorTypeName);
            }

            return errorType;
        }

        protected InfoType GetOrAddInfoTypeByName(string name)
        {
            var infoType =_context.InfoTypes.FirstOrDefault(x => x.Name == name);
            if(null == infoType)
            {
                infoType = new InfoType { Name = name };
                _context.InfoTypes.Add(infoType);
            }

            return infoType;
        }
    }
}
