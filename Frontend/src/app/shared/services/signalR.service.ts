import { EventEmitter, Injectable, OnDestroy, OnChanges, OnInit } from '@angular/core';
import { HubConnection } from '@aspnet/signalr';

import { ChatMessage } from '../models/chatMessage.model';
import { AppConfig } from './../../config/app.config';
import { StockUpdate } from '../models';

@Injectable()
export class SignalRService {
  connected: boolean;
  private _hubConnection: HubConnection;

  stockChanged = new EventEmitter();
  connectionEstablished = new EventEmitter<Boolean>();
  registeredGroup: string;

  constructor() {
    this._hubConnection = new HubConnection(AppConfig.endpoints['signalrHub']);
    // this.start();
    this._hubConnection.onclose( () => this.unRegisterFromGroup());
  }

  public isConnected(): boolean {
    return this.connected;
  }

  public registerToGroup(groupName: string): Promise<any> {
    this.registeredGroup = groupName;
    return this._hubConnection
      .invoke('RegisterOnGroup', this.registeredGroup)
      .then(() => console.log('Registered to group ' + this.registeredGroup));
  }

  public unRegisterFromGroup(): Promise<any> {
    return this._hubConnection
    .invoke('UnRegisterFromGroup', this.registeredGroup)
    .then(() => {
      console.log('Unregistered from group ' + this.registeredGroup);
      this.registeredGroup = null;
    });
  }

  public start(): void {
    if (!this.connected) {
      this._hubConnection
        .start()
        .then(() => {
          console.log('Hub connection started');
          this.connected = true;
          this.connectionEstablished.emit(true);
        })
        .catch(err => {
          console.log('Error while establishing connection');
        });
    }
  }

  public stop(): void {
    if (this.connected) {
      this._hubConnection
        .stop()
        .then(() => {
          console.log('Hub connection stopped');
          this.connected = false;
          this.connectionEstablished.emit(false);
        })
        .catch(err => {
          console.log('Error while terminating connection');
        });
    }
  }

  public registerOnServerEvents(): void {
    this._hubConnection.on('SendMessage', (data: any) => {
      this.stockChanged.emit(data);
    });

    this._hubConnection.on('StockUpdate', (amount: any, type: any) => {
      this.stockChanged.emit(new StockUpdate(amount, type));
    });
  }
}
