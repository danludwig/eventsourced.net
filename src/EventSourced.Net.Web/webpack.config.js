module.exports = {
  entry: [
    './client/index.js'
  ],
  output: {
    path: __dirname + '/wwwroot',
    publicPath: '/',
    filename: 'bundle.js'
  },
  module: {
    loaders: [
      {
        test: /\.jsx?$/,
        exclude: /(node_modules)/,
        loader: 'babel-loader',
        query: {
          presets: ['es2015', 'react'],
          plugins: [
            "syntax-object-rest-spread",
            "transform-object-rest-spread"
          ]
        }
      },
      { test: /\.css$/, loader: "style-loader!css-loader" }
    ]
  }
};
