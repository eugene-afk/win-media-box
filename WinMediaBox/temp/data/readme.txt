NOTE:

Do not change files extensions and names
Copy and save original files before editing if you not familiar with json format

*corePathes.json*

moviesBasePath - path to folder when you store yours video files. For example: D:/movies/
postersBasePath - path to folder with pictures that you want to use as video cover in app, name of image file must be same as video file (excluding extension) and valid image extesions is: ".JPG", ".JPEG", ".PNG". For example: D:/movies/posters/
ipTVPlayerPath - path to your iptv player exe file. For example: D:/Program Files/IPTVPlayer/IPTVPlayer.exe

*RadioStations.json*

img - path to cover for radio station, those pictures must be saved in images folder in subfolder "radio" that located in app root directory, so your path must be looks like that: images/radio/hitfm.png
title - name of radio station that will be used in app. For example: HIT FM
color - color of radio station card in app. Must be in HEX format. For example: #222
uri - path to m3u of radio station in internet, you can find this link in your browser dev-tool in network tab, or watch guides on youtube how to do this. Examle link: http://hitfm.ipfm.net/HitFM?1614301600541

*TwitchChannels.json*

img - path to cover for radio station, those pictures must be saved in images folder in subfolder "twitch" that located in app root directory, so your path must be looks like that: images/twitch/fifa.png
title - name of twitch channel that will be used in app. For example: EA FIFA
color - color of twitch channel card in app. Must be in HEX format. For example: #0AFAB3
channelID - Open stream on twitch.tv that you would like to add and copy link from browser, your link must be looks like that: https://www.twitch.tv/easportsfifa, in that case channel ID will be "easportsfifa"

*YTChannels.json*

img - path to cover for radio station, those pictures must be saved in images folder in subfolder "yt" that located in app root directory, so your path must be looks like that: images/yt/nasa.png
title - name of youtube channel that will be used in app. For example: NASA
color - color of twitch channel card in app. Must be in HEX format. For example: #AA0009
channelID - Open youtube video or stream in your browser, copy link, it must looks like that: https://www.youtube.com/watch?v=21X5lGlDOfg, in that case channel ID will be "21X5lGlDOfg"


If you adding more than one record to json file, it must looks like that:

  {
    "img": "images/radio/hitfm.png",
    "title": "HIT FM",
    "color": "#222",
    "uri": "http://hitfm.ipfm.net/HitFM?1614301600541"
  },
  {
    "img": "images/radio/hitfm.png",
    "title": "HIT FM",
    "color": "#222",
    "uri": "http://hitfm.ipfm.net/HitFM?1614301600541"
  },

