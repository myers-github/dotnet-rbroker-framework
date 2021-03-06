﻿/*
 * PooledSimulation.cs
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
using System.Threading;
using DeployRBroker;
using DeployR;

namespace Pooled
{
    /*
     * PooledSimulation
     *
     * Demonstrates how to simluate, load and test
     * a real-world application.
     *
     * We are going to build a simulation of a
     * Real-Time Scoring Engine for a fictitious Insurance company
     * to integrate into their Customer Support desk system.
     *
     * Steps:
     *
     * 1. Using RBroker
     * 2. And RTaskAppSimulator.
     * 3. Gather customer request information,
     * 4. Push customer request information for analysis to the scoring engine.
     * 5. Make decisions in real-time based on score returned from scoring engine.
     */
    public class PooledSimulation
    {
        static public void Execute()
        {
            try 
            {
                /*
                 * 1. Prepare PooledBrokerConfig instance.
                 *
                 * This example creates an authenticated PooledTaskBroker
                 * to act as our scoring engine.
                 *
                 * The following steps initialize the pool of
                 * R sessions dedicated to our scoring engine.
                 *
                 * In this example, we pre-initialize the workspace
                 * for each R session in the pool to contain the
                 * R model used when scoring customer requests.
                 *
                 * The R model, insurModel.rData, is loaded from
                 * the DeployR repository into each workspace at
                 * startup time. Preloading the R model at startup
                 * reduces runtime overhead and improves overall
                 * response times in the application.
                 */
                RAuthentication rAuth = new RBasicAuthentication(Program.AUTH_USER_NAME, Program.AUTH_PASSWORD);

                PoolCreationOptions poolOptions = new PoolCreationOptions();

                PoolPreloadOptions preloadOptions = new PoolPreloadOptions();
                preloadOptions.filename = Program.TUTORIAL_INSURANCE_MODEL;
                preloadOptions.directory = Program.TUTORIAL_REPO_DIRECTORY;
                preloadOptions.author = Program.TUTORIAL_REPO_OWNER;

                poolOptions.preloadWorkspace = preloadOptions;


                /*
                 * 2. Create RBroker instance.
                 *
                 * The application designer, in conjunction with the
                 * DeployR administrator who provisions and sets
                 * limits on DeployR grid resources, need to decide how
                 * many R sessions will be reserved for the PooledRBroker
                 * used by our scoring engine application.
                 */

                int TARGET_POOL_SIZE = 10;

                PooledBrokerConfig brokerConfig = new PooledBrokerConfig(Program.DEPLOYR_ENDPOINT,
                                                                           rAuth,
                                                                           TARGET_POOL_SIZE,
                                                                           poolOptions);

                Console.WriteLine("About to create pooledTaskBroker.");

                RBroker rBroker = RBrokerFactory.pooledTaskBroker(brokerConfig);

                Console.WriteLine("R session pool size, requested: " +
                        TARGET_POOL_SIZE + " , actual: " +
                        rBroker.maxConcurrency() + "\n");


                /*
                 * 3. Create an RTaskAppSimulator. It will drive
                 * sample customer data scoring requests through the
                 * RBroker.
                 */

                ScoringEngineSimulation simulation = new ScoringEngineSimulation(rBroker);


                /*
                 * 4. Launch RTaskAppSimulator simulation.
                 */

                rBroker.simulateApp(simulation);

                /*
                 * 5. Block until all tasks are complete, and shutdown has been called
                 */
                rBroker.waitUntilShutdown();

            } 
            catch(Exception tex) 
            {
                Console.WriteLine("constructor: ex=" + tex.ToString());
            }

        }
    }
}
