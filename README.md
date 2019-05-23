# Zone.Episerver.PropertyViewer
An Episerver add-on to view multi-lingual properties

This module adds a GUI plugin to the admin section of the Episerver CMS that allows you to navigate the content tree and preview the value of properties across all languages on a single screen.

![Property Viewer 1.1.0](https://i.imgur.com/4m5PRZQ.gif)

## Release notes

### 1.1.0
- Added CMS link for content references
- Added thumbnails and links for Images 
- Added loader when content is loading
- Added rendering for content area items
- Added rendering for link items
- Updated to latest jsTree version
- Updated styling to match standard Epi content tree
- Fixed so that all pages are displayed regardless of master language

### 1.0.0
Initial version

## Limitations
The add-on uses the string representation of a property so will only display helpful information for supported properties (string, bool, int), properties with a comprehensive _ToString_ method, and properties on local blocks.
