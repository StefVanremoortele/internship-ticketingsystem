import { Injectable } from '@angular/core';
import { Http, Response, Headers } from '@angular/http';
import { Observable } from 'rxjs/Observable';
import { AuthService } from '../../shared/services/authentication.service';

@Injectable()
export class LoggingService {

    constructor(private http: Http, private authService: AuthService) { }

    public logUsage(message: string, details: any) {
        const logEntry = this.getTsLoggingInfo(message, details);
        this.LogIt(logEntry, 'Usage');
    }

    public logDiagnostic(message: string, details: any) {
        const logEntry = this.getTsLoggingInfo(message, details);
        this.LogIt(logEntry, 'Diagnostic');
    }

    public logError(message: string, details: any) {
        const logEntry = this.getTsLoggingInfo(message, details);
        if (details._body) {
            logEntry.CorrelationId = details._body.replace(/.*Error ID: /gi, '');
        }
        this.LogIt(logEntry, 'Error');
    }

    public handleApiError(message: string, error: any): any {
        this.logError(message, error);
        throw new Error(error._body ? error._body :  message);
    }

    public startPerfTracker(message: string, details: any) {
        const sessionKey = btoa(message);
        sessionStorage[sessionKey] = JSON.stringify(this.getTsLoggingInfo(message, details));
    }

    public stopPerfTracker(message: string) {
        const sessionKey = btoa(message);
        const now = new Date();

        const contents = sessionStorage[sessionKey];
        if (!contents) {
            return;
        }
        const logEntry = JSON.parse(contents);
        if (logEntry !== null) {
            logEntry.ElapsedMilliseconds = now.getTime() - Date.parse(logEntry.Timestamp);
            this.LogIt(logEntry, 'Performance');
        }
    }

    private LogIt(logEntry: LogEntry, endpoint: string) {
        if (!this.authService.loggedIn) {
            const headers = new Headers();
            headers.append('Content-Type', 'application/json');
            this.http.post('https://loggingapi.knowyourtoolset.com/logging/' + endpoint,
                // subscribe is required -- but we don't have to do anything
                JSON.stringify(logEntry), {headers: headers}).subscribe();
            return;
        }

        // this.authService.getUserInfo().subscribe(userInfo => {
        //     logEntry.UserId = userInfo.sub;
        //     logEntry.UserName = userInfo.email;

        //     for (var property in userInfo) {
        //         if (userInfo.hasOwnProperty(property)) {
        //             logEntry.AdditionalInfo[property] = userInfo[property];
        //         }
        //     }

        //     let headers = new Headers();
        //     headers.append("Content-Type", "application/json");
        //     this.http.post("https://loggingapi.knowyourtoolset.com/logging/" + endpoint,
        //         // subscribe is required -- but we don't have to do anything
        //         JSON.stringify(logEntry), {headers: headers}).subscribe();
        // })
    }

    private getTsLoggingInfo(message: string, additionalInfo: any): LogEntry {
        const logInfo = new LogEntry;
        logInfo.Timestamp = new Date();
        logInfo.Location = window.location.toString();
        logInfo.Product = 'ToDos';
        logInfo.Layer = 'AngularClient';
        logInfo.Message = message;

        logInfo.AdditionalInfo = {};
        if (additionalInfo) {
            for (const property in additionalInfo) {
                if (additionalInfo.hasOwnProperty(property) &&
                        property !== 'ngDebugContext' &&
                        !(additionalInfo[property] instanceof Function)) {
                    logInfo.AdditionalInfo[property] = additionalInfo[property];
                }
            }
        }
        logInfo.AdditionalInfo['user-agent'] = window.navigator.userAgent;

        return logInfo;
    }
}

class LogEntry {
      Timestamp: Date;
      Product: string;
      Layer: string;
      Location: string;
      UserId: string;
      UserName: string;
      Message: string;
      CorrelationId: string;
      ElapsedMilliseconds: number;
      AdditionalInfo: any;
}
