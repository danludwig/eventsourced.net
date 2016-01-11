import React, { Component } from 'react'
import {connect} from 'react-redux'
import TopNav from './TopNav'
import Footer from './Footer'

class Layout extends Component {
  render() {
    const { children } = this.props;
    return (
      <div>
        <TopNav />
        <div className="container body-content">
          {children}
          <Footer />
        </div>
      </div>
    )
  }
}

function select(state) {
  return { };
}

export default connect(select)(Layout)
