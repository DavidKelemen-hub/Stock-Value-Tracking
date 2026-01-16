This project was created inorder to have an application where the user can have a centralized view of SP500 companies' price evolution. 
Currently the data is loaded from a local database, which was populated using the IFinance API which is based on Yahoo Finance records.

<img width="2552" height="1365" alt="image" src="https://github.com/user-attachments/assets/4b05047e-e122-40fc-9306-fb284888adbb" />

The user is able to see a chart with the price evolution of a selected stock across different ranges (5D, 1M, 6M, YTD, 1Y, 5Y, Max (all time data available in the database) ), along with the latest close price recorded. 
Additional indicators include price change, percentage change, highest and lowest prices which are returned based on the selected time range for the given company. 

I thought of how to fill the bottom part of the window, and my idea was to create some kind of ranking system for Top/Low performing stocks based on the selected timeframe - so in the View I added a Grid showing the best and the worst 
performing stocks, ranked by their percentage change (price change is also shown, but criteria for sorting whas the percentage):
<img width="2067" height="592" alt="image" src="https://github.com/user-attachments/assets/c728bec4-d6a9-40c0-8f5c-50a6f0d2879b" />

To change the View from the lowest performing companies to the top performing ones, I decided to just switch a button: 
<img width="2071" height="542" alt="image" src="https://github.com/user-attachments/assets/a25b8bf0-d949-4ffe-a7e8-81ed682a3b58" />

Since the range indicator buttons are triggers for both the stock and performers view, in the case when the "Max" button is pressed to show all time data, the performer view defaults to a 5y range, I chose this to simplify things a bit :). 

The application tracks at the moment 495 companies, I asked Clause to compute a json file containing current SP500 Companies, but the Yahoo Finance API could not retrieve data for all of them, after searching for the faulty companies,
it turned out that they are already delisted. For the moment, I am updating the database using a python script which I ran occasionally, plan is to automate the script to run daily inorder to ease this process. 

As an additional feature, I also integrated the filtering of the list containing the companies, so they can be found more easily. Search can be performed for both symbol (eg. "MSFT") 
or name (eg. "Microsoft"): 


Symbol search: <img width="2550" height="785" alt="image" src="https://github.com/user-attachments/assets/0ccea93f-80f7-44e5-aa61-dcda65267830" />

Name search: <img width="2558" height="784" alt="image" src="https://github.com/user-attachments/assets/4823adb1-0c9e-40c7-bfb4-7ce1a1ebc1db" />

Parsing logic is simple, if the value entered in the search box is contained in any companies' name or symbol, than that company is a match: 
<img width="2542" height="781" alt="image" src="https://github.com/user-attachments/assets/6805510e-6576-40de-bc14-49b9924c3d67" />

This is my first project after a long time using .NET, and the first one with WPF and MVVM architecture. I tried my best to separate the UI from business logic / processing / db service to follow MVVM architecture,
however I am aware of the fact that this project is far from perfect. I already know about a few things that I need to improve on in the future (making use of async/await, proper error handling, more consistent architecture/naming conventions etc.)
but feedback is always appreciated.


