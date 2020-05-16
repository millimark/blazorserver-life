# Blazor server demonstration application 
## Implements a simple algorithm for Conway's game of life

There are three projects in the solution:
1. BlazorServerApp.Shared
	a library that implements the basic data structures and rules of the Life game
2. ConwayWebAPI
	an ASP.NET Web API with a controller that has an interface to the shared library
3. BlazorServerApp
	A Blazor server side app, based on Microsoft's sample, that implements two ways of
	running the Life game:
	- ConwayServer.razor - implements the Life game mechanism directly in the Blazor app
	- ConwayServerWeb.razor - uses the ConwayWebAPI to do the Life game, i.e., it makes 
	a series of calls to the ConwayWebAPI running on a separate server.

To run:
 - use the Visual Studio 2019 Preview (I'm using preview 6, so the "About" box says "Version 16.6.0 Preview 6.0")
 - build the solution
 - run the BlazorServerApp and ConwayWebAPI projects at the same time using the IIS Express debug configuration.
 - try each of the nav links on the left of the home page. One is the version that runs in the server with the naive 
 matrix implementation. The other is the page on which all computation is passed off to
 ConwayWebAPI running on a separate server. 

 Both pages work in much the same way. You can select an initial pattern using the dropdowns,
 or create your own pattern. Click on the Start button to run through the sequence of generations. 
 Click on Stop to stop. Change the speed by sliding the slider. On the version that uses the web API,
 a status line will tell you how long each call takes.

 Note that the matrix is always treated as toroidal, that is, the bottom row wraps around
 to the top row, and the rightmost column wraps around to the leftmost column.
