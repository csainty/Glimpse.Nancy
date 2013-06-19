#Glimpse.Nancy
Uniting two of the best .NET open source projects around!

We have a working proof-of-concept. Now we just need to build out some more complex data tabs for Glimpse to expose all of the goodness inside Nancy.  
Check out the issues board if you want to get involved.

##Installation
No nuget package yet. But if you clone the repository and build the dll. Then it should simply be a matter of  referencing that in your project and adding some web.config that Glimpse uses to enable itself. Something like the following  


    <configSections>  
      <section name="glimpse" type="Glimpse.Core.Configuration.Section, Glimpse.Core, Version=1.0.0, Culture=neutral" />  
    </configuration>  
    <glimpse defaultRuntimePolicy="On" endpointBaseUri="~/Glimpse.axd">
      <logging level="Trace" />
      <runtimePolicies>
        <ignoredTypes>
          <add type="Glimpse.Core.Policy.ControlCookiePolicy, Glimpse.Core" />
        </ignoredTypes>
      </runtimePolicies>
    </glimpse>
