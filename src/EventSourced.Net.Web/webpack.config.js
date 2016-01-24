module.exports = {
  entry: {
    client: './Web/index.js',
    server: './Web/index-server.js'
  },
  output: {
    path: __dirname + '/wwwroot',
    publicPath: '/',
    filename: 'bundle.[name].js'
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
            "transform-class-properties",
            "transform-object-rest-spread"
          ]
        }
      },
      { test: /\.css$/, loader: "style-loader!css-loader" }
    ]
  },
  resolve: {
    extensions: ['', '.js', '.jsx']
  }
};
