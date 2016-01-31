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

        public void AddInfo(string message, string ipAddress, string urlRoute, string infoTypeName)
        {
            var infoType = GetOrAddInfoTypeByName(infoTypeName);
            var info = new Info
            {
                Message = message,
                InfoType = infoType,
                IPAddress = ipAddress,
                UrlRoute = urlRoute,
                Timestamp = DateTime.Now,
            };

            _context.Infoes.Add(info);
            SaveChanges();
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

            error.ErrorType = GetOrAddErrorType(exception.GetType().Name);

            if(null != exception.InnerException)
            {
                error.Errors1.Add(GetErrorFromException(exception.InnerException, storeStackTrace));
            }

            return error;
        }

        //TODO create error type and info types as enumeration tables that inherit from the same interface so we don't have to duplicate logic
        protected ErrorType GetOrAddErrorType(string name)
        {
            var errorType = _context.ErrorTypes.FirstOrDefault(e => e.Name == name);

            if(null == errorType)
            {
                errorType = new ErrorType { Name = name };
                _context.ErrorTypes.Add(errorType);
                //Prevent bug from occuring by adding error type because you cannot save 
                //an new assigned type in the context and on an error at the same time
                //this normally would be fine but if the inner exception is the same new error
                // it will cause duplicates
                _context.SaveChanges();
            }

            return errorType;
        }

        //TODO create error type and info types as enumeration tables that inherit from the same interface so we don't have to duplicate logic
        protected InfoType GetOrAddInfoTypeByName(string name)
        {
            var infoType =_context.InfoTypes.FirstOrDefault(x => x.Name == name);
            if(null == infoType)
            {
                infoType = new InfoType { Name = name };
                _context.InfoTypes.Add(infoType);
                //Prevent bug from occuring by adding error type because you cannot save 
                //an new assigned type in the context and on an error at the same time
                //this normally would be fine but if the inner exception is the same new error
                // it will cause duplicates
                _context.SaveChanges();
            }

            return infoType;
        }
    }
}
