﻿using System;

namespace Binxio.Common
{
    public class XioResult
    {
        public Guid OperationId { get; set; } = Guid.NewGuid();
        public ResultStatus Status { get; set; } = ResultStatus.Pending;
        public string Message { get; set; }

        public XioResult(bool success) => Status = ResultStatus.Success;
        public XioResult(bool success, string message) : this(success) => this.Message = message; 

    }

    public class XioResult<T> : XioResult
    {
        public XioResult(bool success) : base(success)
        {
        }

        public XioResult(bool success, string message) : base(success, message)
        {
        }

        public XioResult(bool success, T resultObject) : base(success) => Result = resultObject;
        public XioResult(bool success, string message, T resultObject) : base(success, message) => Result = resultObject;

        public T Result { get; set; }
    }

}