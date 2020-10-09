using HyperMarket.Errors;
using System;
using System.Collections;
using System.Data.Common;
namespace HyperMarket {
    public class ErrorHelper {
        #region :: common
        public static BaseException Failed() {
            return new BaseException("Operation failed");
        }
        public static BaseException Failed(String message) {
            return new BaseException(message);
        }
        public static BaseException Failed(String source, String message) {
            return new BaseException(message) { Source = source };
        }
        public static BaseException Failed(Exception e) {
            return new BaseException("Operation failed", e);
        }
        public static BaseException Failed(String source, Exception e) {
            return new BaseException("Operation failed", e) { Source = source };
        }
        public static BaseException Failed(String source, String message, Exception e) {
            return new BaseException(message, e) { Source = source };
        }
        public static BaseException Failed(String source, String message, Exception e, IDictionary data) {
            var be = new BaseException(message, e) { Source = source };
            if (data != null)
                foreach (var item in data.Keys)
                    be.Data.Add(item, data[item]);
            return be;
        }
        public static Exception FormatException() {
            return new FormatException();
        }
        public static Exception FormatException(String message) {
            return new FormatException(message);
        }
        public static Exception FormatException(String message, Exception inner) {
            return new FormatException(message, inner);
        }
        public static SmtpException Smtp(String message, Exception inner) {
            return new SmtpException(message, inner);
        }
        public static SmtpException Smtp(String message) {
            return new SmtpException(message);
        }
        public static SmtpException Smtp() {
            return new SmtpException();
        }
        #endregion
        #region :: notallowed
        public static DuplicateEntryException Duplicate(String message) {
            return new DuplicateEntryException(message);
        }
        public static NotAllowedException NotAllowed(String message) {
            return new NotAllowedException(message);
        }
        public static AuthException AuthFail(String message) {
            return new AuthException(message);
        }
        public static AuthException AuthFail(String message, Exception inner) {
            return new AuthException(message, inner);
        }
        #endregion
        #region :: notfound
        public static NotFoundException NotFound(string message) {
            return new NotFoundException(message);
        }
        public static NotFoundException NotFound<T>() {
            return new NotFoundException(typeof(T).Name);
        }
        #endregion
        #region :: arguments
        public static ArgumentException Arg(String argName) {
            return new ArgumentException("Invalid argument", argName);
        }
        public static ArgumentException Arg(String argName, String message) {
            return new ArgumentException(message, argName);
        }
        public static ArgumentNullException ArgNull(String argName) {
            return new ArgumentNullException(argName);
        }
        public static ArgumentNullException ArgNull(String argName, String message) {
            return new ArgumentNullException(argName, message);
        }
        public static ArgumentOutOfRangeException ArgRange() {
            return new ArgumentOutOfRangeException();
        }
        public static ArgumentOutOfRangeException ArgRange(String argName) {
            return new ArgumentOutOfRangeException(argName);
        }
        public static ArgumentOutOfRangeException ArgRange(String argName, String message) {
            return new ArgumentOutOfRangeException(argName, message);
        }
        public static ArgumentOutOfRangeException ArgRange(String argName, Object actualValue, String message) {
            return new ArgumentOutOfRangeException(argName, actualValue, message);
        }
        #endregion
        #region :: persistence
        public static PersistenceException Persistence(String message, Exception e) {
            return new PersistenceException(message, e);
        }
        public static PersistenceException Persistence(DbException e) {
            return new PersistenceException("Query error", e);
        }
        #endregion
        #region :: invalid operation
        public static InvalidOperationException InvalidOperation(String message, Exception e) {
            return new InvalidOperationException(message, e);
        }
        public static InvalidOperationException InvalidOperation(String message) {
            return new InvalidOperationException(message);
        }
        public static InvalidOperationException InvalidOperation() {
            return new InvalidOperationException();
        }
        public static InvalidArgumentException InvalidArgument() {
            return new InvalidArgumentException();
        }
        public static InvalidArgumentException InvalidArgument(String message) {
            return new InvalidArgumentException(message);
        }
        public static InvalidArgumentException InvalidArgument(String message, Exception ex) {
            return new InvalidArgumentException(message, ex);
        }
        public static InvalidImageException InvalidImage() {
            return new InvalidImageException();
        }
        public static InvalidImageException InvalidImage(String message) {
            return new InvalidImageException(message);
        }
        public static InvalidImageException InvalidImage(String message, Exception ex) {
            return new InvalidImageException(message, ex);
        }
        public static InvalidStateException InvalidState() {
            return new InvalidStateException();
        }
        public static InvalidStateException InvalidState(String message) {
            return new InvalidStateException(message);
        }
        public static InvalidStateException InvalidState(String message, Exception ex) {
            return new InvalidStateException(message, ex);
        }
        #endregion
        #region :: Not supported
        public static NotSupportedException NotSupported(String message) {
            return new NotSupportedException(message);
        }
        public static NotSupportedException NotSupported() {
            return new NotSupportedException();
        }
        #endregion
        #region :: External exception
        public static NoConnectionException NoConnect() {
            return new NoConnectionException();
        }
        public static NoConnectionException NoConnect(String message) {
            return new NoConnectionException(message);
        }
        public static NoConnectionException NoConnect(String message, Exception inner) {
            return new NoConnectionException(message, inner);
        }
        public static DatabaseException DatabaseException(String message, Exception ex) {
            return new DatabaseException(message, ex);
        }
        public static MailSendingException MailFail(String message, Exception ex) {
            return new MailSendingException(message, ex);
        }
        public static RemoteServerProcessingException Remote() {
            return new RemoteServerProcessingException();
        }
        public static RemoteServerProcessingException Remote(String message) {
            return new RemoteServerProcessingException(message);
        }
        public static RemoteServerProcessingException Remote(String message, Exception inner) {
            return new RemoteServerProcessingException(message, inner);
        }
        #endregion
        
    }
}