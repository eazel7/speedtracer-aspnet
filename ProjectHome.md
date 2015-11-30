# Introduction #

SpeedTracer is a Google Chrome extension for tracing client and server side timings. SpringSource TCServer and Google App Engine AppStats for Java applications comes with integrated support for SpeedTracer so you can see how your server-side timings delay your request.
This project enables the same kind of server side trace support for ASP.NET projects.

# Details #

Currently, this is an initial spike, but you can reuse it in your projects.

  * Don't use this version in production, as it will indefinitely add objects into the HttpApplication object bag.
  * Syntax used is currently like:

using (Tracer.Trace ("My trace message")
{
> // ... code here
}
  * I want to integrate it with diagnostics information in a future (like the one you use to write into the Windows event log)
  * I'm not showing related source code line numbers, I plan to do so if possible