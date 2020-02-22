# TDH201-server
This project was build with .Net Core 3.1.

To run project: 
1. Visit https://github.com/dotnet/core/blob/master/README.md and make sure you meet the requirements.
2. Clone the project
3. Run locally

About the Project:

The project was built as part of the Digital Humanities course as part of the undergraduate degree in computer science at Ben Gurion University, Beer Sheva: https://www.cs.bgu.ac.il/~tdh201/Main

This is the server side of a project that aims to show the development of the city of Beer Sheva throughout history, on a map. URL: https://tdh201-cfc16.firebaseapp.com

As part of our server-side work, we created a .NET Core project in C#, presented here.

We worked with the Beer Sheva Municipality's OpenData API at: https://www.beer-sheva.muni.il/OpenData/Pages/Data.aspx

We contacted various repositories on this API, including: House numbers and street names, Neighborhoods names and more.
  
We used fields such as: When was the street approved by city councils, in what neighborhood or whether the street is named after a woman, to display information that we thought would be of interest to the project's viewers, and interested us as city dwellers.

We discovered that there is more information in the database that can be used and displayed, such as city playgrounds, dog gardens, gas stations, etc. We were glad we built the foundation for more people to use what we have already created and expand the project easily, by accessing the desired repository on the municipality's site and adding it to the existing project.

The information that came from the Be'er Sheva municipality database was in the form of GPS coordinates, so we needed used two more APIs to process the information so that we could display it on a client-side map.
  
We used the OpenStreetMap as well as Google Maps googleapis API's.

We found the information in OpenStreetMap to be more accurate and efficient (returns a street coordinate sequence rather than a single dot / point), so we preferred to contact it first, and only the street name is not in this repository, we turned to the Google API.

After processing the data, we put it in easy-to-use Json form, both for our site and as a public service.

Request To Get Data:
Get Request to:

