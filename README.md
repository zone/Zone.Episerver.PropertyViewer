# Zone.Episerver.PropertyViewer
An Episerver add-on to view multi-lingual properties

This module adds a GUI plugin to the admin section of the Episerver CMS that allows you to navigate the content tree and preview the value of properties across all languages on a single screen.

![alt text](https://i.imgur.com/7ygWvQt.gif)

## Limitations
The add-on uses the string representation of a property so will only display helpful information for simple properties (string, bool, int), properties with a comprehensive _ToString_ method, and properties on local blocks.
