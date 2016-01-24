import React, { Component, PropTypes } from 'react'
import Helmet from 'react-helmet'

export default class NotFound404 extends Component {
  render() {
    return(
      <div>
        <Helmet title="404 Not Found" />
        <h1 className="text-danger">Error.</h1>
        <h2 className="text-danger">404 Not Found</h2>
      </div>
    )
  }
}
