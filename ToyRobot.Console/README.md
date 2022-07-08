# ToyRobot.Console

Console application for ToyRobot project.

You control a robot by typing commands on a console.
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
