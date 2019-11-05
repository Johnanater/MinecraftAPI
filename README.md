MinecraftAPI
===========

*A simple Web API to retrieve Minecraft player data*

---

**Using in a Minecraft client:**

If you want to use this API as a "skinfix" for Minecraft Legacy versions, you need
a skin fix mod. I'd recommend mine_diver's SkinFixUni mod found [here](https://wiki.modification-station.net/doku.php?id=mods:skinfixuni)!

By default, the mod does not use the API by default. You can change `mod_SkinFix.cfg` to my server:

```
Skin\ URL=http\://icebergcraft.com\:6543/api/Minecraft/GetSkinByUsername/?username\=
Cape\ URL=http\://icebergcraft.com\:6543/api/Minecraft/GetCapeByUsername/?username\=
```

---

**Using:**

```
GetUUID/?username=<username>
Will return the UUID of a player

GetSkin/?uuid=<uuid>
Will return the player's skin in png format (if they have one)

GetSkinByUsername/?username=<username>
Will return the player's skin in png format (if they have one)

GetCape/?uuid=<uuid>
Will return the player's cape in png format (if they have one)

GetCapeByUsername/?username=<username>
Will return the player's cape in png format (if they have one)
```

For example:

The URL:

`https://icebergcraft.com/api/Minecraft/GetSkinByUsername/?username=Johnanater`

Will return a png of my skin.

---

**Building:**

To build, just open MinecraftAPI.sln with Rider or Visual Studio and hit "build"
 
---

**Running:**

`dotnet MinecraftAPI.dll [options]`
 
---

**Parameters:**

To edit the site's URL or port, use the parameter:

`--url http://localhost:5000`

To edit the cache time (in seconds), use the parameter

`--cache-time=1800`

---

**Help:**

If you still need help feel free to contact me on Discord: Johnanater#6836

or on my Discord server: https://discord.gg/VTCzMVG

---	

**Love my work?**

Ethereum: 0x43db5a4a44a57f0699c320dbf1131879ec831274

Ripple: rDrdhCVD79js6dTWHC1d6cdHjvj2hD3T1H

[![](https://www.paypalobjects.com/webstatic/en_US/btn/btn_donate_cc_147x47.png)](https://www.paypal.com/cgi-bin/webscr?cmd=_s-xclick&hosted_button_id=7QEHYC457X5SW)
