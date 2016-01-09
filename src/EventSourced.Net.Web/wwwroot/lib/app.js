import '../css/site.css';

import $ from 'jquery';
import login from './users/login/login';
import register from './users/register/register';
import redeem from './users/register/redeem';
import verify from './users/register/verify';

let path = location.pathname;
let view = null;

if (path === '/login')
  view = login;
else if (path === '/register')
  view = register;
else if (/^\/register\/.*\/redeem/.test(path))
  view = redeem;
else if (/^\/register\/.*/.test(path))
  view = verify;

if (view !== null) {
  $(view);
}
