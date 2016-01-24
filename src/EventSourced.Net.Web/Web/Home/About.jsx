import React, { Component, PropTypes } from 'react'
import Helmet from 'react-helmet'

export default class About extends Component {
  render() {
    return (
      <div>
        <Helmet title="About" />
        <h2>About.</h2>
        <h3></h3>
        <p>Use this area to provide additional information.</p>
      </div>
    )
  }
}
