import React, { Component, PropTypes } from 'react'
import {connect} from 'react-redux'
import Layout from '../../Web/Home/Layout'
import Initializing from './Initializing'

class App extends Component {
  render() {
    const { initialized, children } = this.props
    return (
      <div>
        {initialized ?
          <Layout>
            {children}
          </Layout> :
          <Initializing />
        }
      </div>
    )
  }

  static get propTypes() {
    return {
      initialized: PropTypes.bool.isRequired
    }
  }
}

function select(state) {
  return {
    initialized: state.app.data.server.initialized
  }
}

export default connect(select)(App)
