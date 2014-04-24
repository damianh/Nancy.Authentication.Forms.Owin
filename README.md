An example OWIN middleware that deciphers a Nancy forms auth cookie and puts a ClaimsPrincipal in the OWIN environment. This allows you authenticate a user with Nancy's Forms Authentication, and be able to identify that user in other middleware or frameworks, such as signalr.

Parts of note:

 1. The [Startup](https://github.com/damianh/Nancy.Authentication.Forms.Owin/blob/master/src/Nancy.Authentication.Forms.Owin.Tests/NancyApp/Startup.cs) class where we _externally_ configure Nancy's Forms Auth crypto. We need this configuration in Nancy to encrypt the cookies and in our middleware to decypt it. Read more on [Nancy's crypto](https://github.com/NancyFx/Nancy/wiki/Forms-Authentication#a-word-on-cryptography).
 2. I use ClaimsPrincipal in the middleware and share that with the other owin middleware via "server.User" key. This is in line with other OWIN security middleware and povides compatiblity with frameworks that are aware of this.
 3. You need to provide an implementation of IClaimsPrincipalLookup to convert the Nancy guid user id stored in the nancy auth cookie to a ClaimsPrincipal to be stored in the owin environment dictionary.
 4. While you can still use Nancy's Context.CurrentUser _within_ your Nancy app, there is an extension method to get the ClaimsPrincipal : Context.GetClaimsPrincipal()

Suggestions, feedback, pull requests welcome.

For now, you'll have to copy/paste the projects / code. If there is enough interest, then I'll create a NuGet package.

[@randompunter](http://twitter.com/randompunter)
