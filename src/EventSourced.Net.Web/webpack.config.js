module.exports = {
  entry: [
    './client/babel/index.js'
  ],
  output: {
    path: __dirname + '/webroot',
    publicPath: '/',
    filename: 'bundle.js'
  }
};
