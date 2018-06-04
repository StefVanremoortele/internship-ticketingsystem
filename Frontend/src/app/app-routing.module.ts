import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { AuthService, AuthGuardService} from './shared';
import { AccountComponent } from './account';
import { CartComponent } from './cart';
import { EventsComponent, EventDetailsComponent } from './events';
import { HomeComponent } from './home';
import { MyTicketsComponent } from './my-tickets';
import { NotFoundComponent, UnauthorizedComponent } from './core';
import { OrderHistoryComponent } from './order-history';


const routes: Routes = [
  { path: '', redirectTo: '/home', pathMatch: 'full' },
  { path: 'home', component: HomeComponent },
  { path: 'events', component: EventsComponent },
  { path: 'events/:id', component: EventDetailsComponent },

  { path: 'cart', component: CartComponent, canActivate:[AuthGuardService]},
  { path: 'my-tickets', component: MyTicketsComponent, canActivate:[AuthGuardService]},
  { path: 'account', component: AccountComponent, canActivate:[AuthGuardService]},
  { path: 'order-history', component: OrderHistoryComponent, canActivate:[AuthGuardService]},

  { path: 'unauthorized', component: UnauthorizedComponent },
  { path: '**', component: NotFoundComponent }
];

export const authProviders = [
  AuthService,
  AuthGuardService
];

export const routing = RouterModule.forRoot(routes);
