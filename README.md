# 🌦️ Daily Weather Console Notifier

This simple C# console app fetches the daily weather forecast at **00:00** every day and gives you personalized advice like “grab a jacket” or “stay hydrated 🔥”.

> ⚠️ You’ll need a free API key from [Visual Crossing Weather](https://www.visualcrossing.com/) to use it.  
> Full Visual Crossing documentation: https://www.visualcrossing.com/resources/documentation/weather-api/timeline-weather-api/
>
> ⚠️ You'll also need a free API key from [IPStack](ipstack.com) to determine location automatically.
> Full IPStack documentation: https://ipstack.com/documentation

---

## ✅ What to do before running

-  🔑 Grab your **API key** from Visual Crossing and IPStack
---

## 🔔 Future Ideas 

- [x] 📡 City based on your location (you wont need to change 'city' it will do it automatically)
- [x] 💬 Add telegram bot that can announce weather notifications
- [ ] 💬 Add voice alerts (say the weather aloud using a TTS lib)
- [ ] 📲 Make a phone app so it can send notifications every day to your phone
- [x] ✅ Add error handling to make the program more stable

## Structure
```
ConsoleWeatherApp/
│
├── ConsoleWeatherApp.csproj # Project file
├── Program.cs # Entry point
│
├── Services/ # 💡 Logic and helpers
│ └── WeatherService.cs # Handles weather fetching and output
| └── LocationService.cs # Handles location services
```
---

Made by Max ;)
