var webpack = require('webpack');
var path = require('path');

module.exports = {
  devtool: 'eval',
  entry: {
    login: './js/users/login/login',
    redeem: './js/users/register/redeem',
    register: './js/users/register/register',
    verify: './js/users/register/verify'
  },
  output: {
    path: path.join(__dirname, 'wwwroot/js'),
    filename: '[name].js'
  },
  module: {
    loaders: [
      {
        test: /\.jsx?$/,
        exclude: /(node_modules|bower_components)/,
        loader: 'babel', // 'babel-loader' is also a legal name to reference
        query: {
          presets: ['react', 'es2015']
        }
      }
    ]
  }
};
