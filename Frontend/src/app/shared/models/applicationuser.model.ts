export class ApplicationUser {
  constructor(

    public userId: string,
    public email: string,
    public firstname?: string,
    public lastname?: string,
    public username?: string
  ) {

}

}
// export class ApplicationUser {
//   userId: string;
//   email: string;
//   firstname?: string;
//   lastname?: string;
//   username?: string;

//   constructor(

//     userId: string,
//     email: string,
//     firstname?: string,
//     lastname?: string,
//     username?: string
//   ) {

//     this.userId = userId;
//     this.email = email;
//     this.firstname = firstname;
//     this.lastname = lastname;
//     this.username = username;
// }

// }
