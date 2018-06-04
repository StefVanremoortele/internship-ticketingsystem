import {
  animate,
  state,
  style,
  transition,
  trigger
} from '@angular/animations';

export const fadeInOut = trigger('fadeInOut', [
  transition(':enter', [
    style({ opacity: 0 }),
    animate('0.4s ease-in', style({ opacity: 1 }))
  ]),
  transition(':leave', [animate('0.4s 10ms ease-out', style({ opacity: 0 }))])
]);

export const enterLeave = trigger('enterLeave', [
  // :ENTER TRANSITION
  // Transition Styles
  transition('void => *', [
    // 'From' styles
    style({
      opacity: 0.5,

      transform: 'scale(0.8)'
    }),
    animate(
      '550ms ease-out',
      // 'To' styles
      // 1 - Comment this to remove the item's grow...
      style({
        opacity: 1,
        transform: 'scale(1)'
      })
    )
  ])
]);

export function flyInOut(duration: number = 0.2) {
  return trigger('flyInOut', [
    state('in', style({ opacity: 1, transform: 'translateX(0)' })),
    transition('void => *', [
      style({ opacity: 0, transform: 'translateX(-100%)' }),
      animate(`${duration}s ease-in`)
    ]),
    transition('* => void', [
      animate(
        `${duration}s 10ms ease-out`,
        style({ opacity: 0, transform: 'translateX(100%)' })
      )
    ])
  ]);
}
