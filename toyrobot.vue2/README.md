# toyrobot.vue2

Vue application for ToyRobot project.

You control a robot by selecting commands in the ui.

Initial state: the robot is outside an existing map

Valid commands
- CREATEMAP width,height : create a new map of selected dimensions with no robots in it
- CREATEROBOT : create a new robot on the current map
- PLACE x,y,direction(NORTH,SOUTH,EAST,WEST): move the current robot to a location on the map with the selected orientation
- LEFT : rotate left the robot by 90 degrees
- RIGTH : rotate right the robot by 90 degrees
- MOVE : move forward the robot by 1 cell
- REPORT : show the current location and orientation of the robot
- SIZE : show the current map size

## Automatic code generation

Code and objects needed for the API calls are generated using openapi generator tool.
Generated code is in gen/toyRobotApi

## Project setup

```
npm install --global yarn
yarn install
```

### Compiles and hot-reloads for development

```
yarn serve
```

### Compiles and minifies for production

```
yarn build
```

### Run open api generator tool openapi-generator-cli

```
yarn api
```

### Lints and fixes files

```
yarn lint
```

### Customize configuration

See [Configuration Reference](https://cli.vuejs.org/config/).
