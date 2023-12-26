//=====================================================================================
// Developed by Kallebe Lins (https://github.com/kallebelins)
//=====================================================================================
// Reproduction or sharing is free! Contribute to a better world!
//=====================================================================================
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Mvp24Hours.Extensions;
using Mvp24Hours.Helpers;
using System.Threading.Tasks;
using Xunit;
using Xunit.Priority;
using Mvp24Hours.WebAPI.Extensions;
using Mvp24Hours.WebAPI.Middlewares;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.Extensions.Primitives;
using System.Collections.Generic;
using Moq;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.TestHost;
using System.Net;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using System;
using System.Net.Http;
using Microsoft.AspNetCore.DataProtection.KeyManagement;

namespace Mvp24Hours.Patterns.Test
{
    /// <summary>
    /// 
    /// </summary>
    [TestCaseOrderer(PriorityOrderer.Name, PriorityOrderer.Name)]
    public class ApplicationBuilderTest
    {
        [Fact]
        public async Task TestExceptions1()
        {
            // arrange
            using var host = await new HostBuilder()
                .ConfigureWebHost(webBuilder =>
                {
                    webBuilder
                        .UseTestServer()
                        .ConfigureServices(services =>
                        {
                            services.AddRouting();
                            services.AddMvp24HoursWebExceptions(x =>
                            {
                                x.TraceMiddleware = false;
                            });
                        })
                        .Configure(app =>
                        {
                            app.UseRouting();
                            app.UseMvp24HoursExceptionHandling();
                            app.UseEndpoints(endpoints =>
                            {
                                endpoints.MapGet("/", (_) => { throw new System.Exception(); });
                            });
                        });
                })
                .StartAsync();

            // act
            var response = await host.GetTestClient().GetAsync("/");

            // assert
            Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
        }

        [Fact]
        public async Task TestExceptions2()
        {
            // arrange
            using var host = await new HostBuilder()
                .ConfigureWebHost(webBuilder =>
                {
                    webBuilder
                        .UseTestServer()
                        .ConfigureServices(services =>
                        {
                            services.AddRouting();
                            services.AddMvp24HoursWebExceptions(x =>
                            {
                                x.TraceMiddleware = false;
                                x.StatusCodeHandle = (Exception exception) =>
                                {
                                    return exception is ArgumentNullException ? (int)HttpStatusCode.BadRequest : (int)HttpStatusCode.InternalServerError;
                                };
                            });
                        })
                        .Configure(app =>
                        {
                            app.UseRouting();
                            app.UseMvp24HoursExceptionHandling();
                            app.UseEndpoints(endpoints =>
                            {
                                endpoints.MapGet("/", (_) => { throw new ArgumentNullException(); });
                            });
                        });
                })
                .StartAsync();

            // act
            var response = await host.GetTestClient().GetAsync("/");

            // assert
            Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
        }

        [Fact]
        public async Task TestCors1()
        {
            // arrange
            using var host = await new HostBuilder()
                .ConfigureWebHost(webBuilder =>
                {
                    webBuilder
                        .UseTestServer()
                        .ConfigureServices(services =>
                        {
                            services.AddRouting();
                            services.AddMvp24HoursWebCors(x =>
                            {
                                x.AllowAll = true;
                                x.AllowRequestOptions = true;
                            });
                        })
                        .Configure(app =>
                        {
                            app.UseRouting();
                            app.UseMvp24HoursCors();
                            app.UseEndpoints(endpoints =>
                            {
                                endpoints.MapGet("/", async context =>
                                {
                                    await context.Response.WriteAsync($"Running!");
                                });
                            });
                        });
                })
                .StartAsync();

            // act
            var response = await host.GetTestClient().GetAsync("/");

            // assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task TestCors2()
        {
            // arrange
            using var host = await new HostBuilder()
                .ConfigureWebHost(webBuilder =>
                {
                    webBuilder
                        .UseTestServer()
                        .ConfigureServices(services =>
                        {
                            services.AddRouting();
                            services.AddMvp24HoursWebCors(x =>
                            {
                                x.AllowAll = false;
                                x.AllowRequestOptions = false;
                            });
                        })
                        .Configure(app =>
                        {
                            app.UseRouting();
                            app.UseMvp24HoursCors();
                            app.UseEndpoints(endpoints =>
                            {
                                endpoints.MapGet("/", async context =>
                                {
                                    if (!context.Response.Headers.ContainsKey("Access-Control-Allow-Headers"))
                                    {
                                        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                                    }
                                    await context.Response.WriteAsync($"Running!");
                                });
                            });
                        });
                })
                .StartAsync();

            // act
            var response = await host.GetTestClient().GetAsync("/");

            // assert
            Assert.Equal(HttpStatusCode.InternalServerError, response.StatusCode);
        }

        [Fact]
        public async Task TestCorrelationId1()
        {
            // arrange
            using var host = await new HostBuilder()
                .ConfigureWebHost(webBuilder =>
                {
                    webBuilder
                        .UseTestServer()
                        .ConfigureServices(services =>
                        {
                            services.AddRouting();
                        })
                        .Configure(app =>
                        {
                            app.UseRouting();
                            app.UseMvp24HoursCorrelationId();
                            app.UseEndpoints(endpoints =>
                            {
                                endpoints.MapGet("/", async context =>
                                {
                                    if (context.TraceIdentifier != "123456")
                                    {
                                        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                                    }
                                    await context.Response.WriteAsync($"Running!");
                                });
                            });
                        });
                })
                .StartAsync();

            // act
            var client = host.GetTestClient();
            client.DefaultRequestHeaders.TryAddWithoutValidation("X-Correlation-ID", "123456");
            var response = await client.GetAsync("/");

            // assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
