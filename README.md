# Win Media Box
## Description
Win Media Box is a media application for Windows platform include possibility to watch IP-TV, listen radio, watch YouTube, Twitch and local video files. This application developed on .NET platform for desktops (WPF). I wrote this solution for personal use and mainly for my grandparents and therefore application is very easy to use and can be controlled by a remote controller ([like this](https://content2.rozetka.com.ua/goods/images/big/8861120.jpg)). I decided to create my own solution because I don't found a better application that would be laconic, simple enough and suit my needs, also I wanted to keep the functionality of a regular Windows PC.

## Preview
<p align="center">
  <img src="preview/543cfd1ea97fe47447073db8f1a925c7d68cd53a.gif" width="auto">
  <img src="preview/91feb75976eabe3e30b64b84e4c0f88fba2c2421.gif" width="auto">
</p>

## Notes and features
* **Windows API**. Used by app for various interactions like send keys, shortcuts and power control. 
* **IP-TV**. To use this module you need pre-installed ip-tv player ([for example](https://borpas.info/iptvplayer)). Application just control open, close and can resize player, nothing special. Also available experimental option to use vlc library for playing iptv.
* **Radio**. For work with radio streams I used WPF MediaElement component.
* **Local media**. For now Local Storage module works only with files in video formats. I prefer to use as default player pre-installed in Windows (it's used in app by default). For this case you may need some extra configs for this player like enable always fullscreen. Also available experimental option to use vlc library for playing video content. To use "series" feature you can create folder inside the local media directory with prefix "series_" in name, then it would be separate folder in the local media module, series are switching automatically.
* **WEB resources integration**. For YouTube and Twitch integration I decided to use embed players with WPF WebView component.
* **Customization**. User can customize appearance of app by edditing some settings in config json files and image files (like background of main screen, cards color, etc.)
* **Content management**. This part is not very easy to use, for now all management of content can be done by edditing json config files.
* **Instructions**. Application folders with edditable images and json config files contains readme text files with detailed instructions.
* **Power management**. Power menu allows to shutdown PC, go to sleep or use timer for those options.

## Todos
Although in current state this application match my needs and works stable enought in real world, for now it's not a fully finished project that need some important things to do:

* **Hotkeys on cancel/resume session**. With .net6/win11 appeared some bugs with unregister hotkeys in sleep.
* **VLC time**. Add timeline for the player and possibility to show/hide current time for video.
* **Database**. Add database core and as a first feature add a history of the user viewing to restore viewing from the moment it was stopped.
* **Content management**. For now user need to edit json config files to manage content, but it's not very comfortable and need to be done from app functionality.
* **Settings**. As in previous point to edit some of customizeble things user need to work with app filesystem and manual replace files etc. So a better way will be to add this functionality inside app. Also this module can include some properties that for now hardcoded in app like remote controller buttons events, some other default properties, pathes etc.
* **M3U Parser**. The current parser for m3u playlists is a bit not universal, so more work will have to be done on that.
* **Input controls improvement of vlc components**. For now application can handle only very basic commands, this functionality can be expanded further.
* ~~**Memory optimizations**. App has some problems with RAM usage, due to some not the most optimal constructions in business logic usage of RAM can be excessive.~~ note: after some observation and testing, no critical memory issues were noticed, but a small optimization was done.
* ~~**Added VLC support** for iptv and local media~~
* ~~**Player selection window for playing media files and iptv**. This choice had to be added after adding the vlc libraries in app.~~
