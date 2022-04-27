import { UserApi, CommandApi, Configuration } from 'toy-robot-axios';

export class UserApiWrapper extends UserApi{
    constructor(){
        super();
        this.configuration = new Configuration() 
        this.configuration.basePath =  (window as any).toyRobotApi;
        console.log("apibasepath", (window as any));
    }
}

export class CommandApiWrapper extends CommandApi{
    constructor(){
        super();
        this.configuration = new Configuration() 
        this.configuration.basePath =  (window as any).toyRobotApi;
    }
}