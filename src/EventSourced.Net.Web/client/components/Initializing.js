import React, { Component, PropTypes } from 'react'
import { connect } from 'react-redux'
import Footer from '../../Web/Home/Footer'

class Initializing extends Component {
  render() {
    const { initialized, unavailable } = this.props
    return (
      <div>
        <div className="navbar navbar-inverse navbar-fixed-top">
          <div className="container">
            <div className="navbar-header">
              <span className="navbar-brand">EventSourced.Net is
                {' '}
                {unavailable ? 'unavailable' : 'loading...'}
              </span>
            </div>
          </div>
        </div>
        { unavailable ?
          <div className="container body-content">
            <h1 className="text-danger">No initial state.</h1>
            <h2>It{"'"}s an /api problem.</h2>
            <h3>Go fix the code now.</h3>
            <Footer />
          </div>
          : ''
        }
      </div>
    )
  }
}

Initializing.propTypes = {
  initialized: PropTypes.bool.isRequired,
  unavailable: PropTypes.bool.isRequired
}

function select(state) {
  return {
    initialized: state.app.data.server.initialized,
    unavailable: state.app.data.server.unavailable
  }
}

export default connect(select)(Initializing)
