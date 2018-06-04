import { InjectionToken } from '@angular/core';

import { IAppConfig } from './iapp.config';

export const apiUrl = 'http://localhost:5001/api';

export let APP_CONFIG = new InjectionToken('app.config');

export const AppConfig: IAppConfig = {
  loggedInRoutes: [
    {
      name: 'Home',
      routerLink: 'home'
    },
    {
      name: 'Events',
      routerLink: 'events'
    },
    {
      name: 'Cart',
      routerLink: 'cart'
    },
    {
      name: 'My Tickets',
      routerLink: 'my-tickets'
    },
    {
      name: 'Order History',
      routerLink: 'order-history'
    },
    {
      name: 'Account',
      routerLink: 'account'
    }
  ],
  loggedOutRoutes: [
    {
      name: 'Home',
      routerLink: 'home'
    },
    {
      name: 'Events',
      routerLink: 'events'
    }
  ],
  endpoints: {
    api: apiUrl,
    events: apiUrl + '/events',
    users: apiUrl + '/users',
    orders: apiUrl + '/account/orders',
    tickets: apiUrl + '/account/tickets',
    account: apiUrl + '/account',
    // signalrHub: 'http://ticketingsystem-api.eastus.cloudapp.azure.com/coolmessages'
    signalrHub: 'http://localhost:5001/coolmessages'
  },
  constants: {
    angularClientUrl: 'http://localhost:4200',
    authorityUrl: 'http://localhost:5000'
  }
};
