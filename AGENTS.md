This is a very basic overview of this project.

# Overview

This is a Football Manager style game about being a Director of Football at a club.
The main gameplay revolves around making decisions, some initiated by the player (e.g. sacking the coach) to some coming up randomly (e.g. player troubles).
The special thing about this game is that there is an LLM behind the scenes, making the players/coaches etc feel more alive and having basic reasoning.

# Project Setup

The project is broken up into server and client side projects.
At a high level, there is a React web app that presents the game, and it talks to a C#-based server that manages the logic, handles actions, and interfaces with the LLM.

The primary setup will be a local deployment of the UI and server running together. But in the future, other desktop platforms and mobile will be supported. It's possible for platforms or systems where a local LLM is not supported, the server is cloud-hosted and only the UI is shipped.

## Solution Overview

### footballdirector.client

This is the react-based frontend

### FoodballDirector.Server

This is an ASP.NET server host. Game logic does not go here, it is just the web server.
It exposes OpenAPI.

### FootballDirector.Core

This is where the main game logic is contained.

### FootballDirector.Contracts

This is a dedicated project to house the Request/Response DTOs

### FootballDirector.Desktop

This is a Windows-friendly shell with WPF + WebView2. In the future, other similar projects can be created to expose Linux/Mac friendly shells, or mobile shells.
