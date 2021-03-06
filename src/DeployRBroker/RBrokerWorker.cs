﻿/*
 * RBrokerWorker.cs
 *
 * Copyright (C) 2010-2014 by Revolution Analytics Inc.
 *
 * This program is licensed to you under the terms of Version 2.0 of the
 * Apache License. This program is distributed WITHOUT
 * ANY EXPRESS OR IMPLIED WARRANTY, INCLUDING THOSE OF NON-INFRINGEMENT,
 * MERCHANTABILITY OR FITNESS FOR A PARTICULAR PURPOSE. Please refer to the
 * Apache License 2.0 (http://www.apache.org/licenses/LICENSE-2.0) for more details.
 *
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DeployRBroker
{
    /// <summary>
    /// Represents any R Analytics worker for execution on an RBroker instance
    /// </summary>
    /// <remarks></remarks>
    public interface RBrokerWorker
    {

        /// <summary>
        /// Function that executes the task using the DeployR Client Libary API
        /// </summary>
        /// <returns>RTaskResult reference</returns>
        /// <remarks></remarks>
        RTaskResult call();
    }
}
