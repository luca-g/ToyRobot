import { ICommandText } from 'toy-robot-axios'
import {CommandAndResult} from './model/commandAndResult'
export type AppState = {
    userLogins: string[],
    currentUser: string | null,
    currentUserToken: string | null,
    currentMapId: number | null,
    currentRobotId: number | null,
    commandAndResults: CommandAndResult[],
    commands: ICommandText[],
}