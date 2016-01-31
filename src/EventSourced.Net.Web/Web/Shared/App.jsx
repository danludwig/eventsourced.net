import React, { Component, PropTypes } from 'react'
import { Provider } from 'react-redux'
import { Router, Route, IndexRoute } from 'react-router'
import Layout from './Layout'
import Home from '../Home/Home'
import About from '../Home/About'
import Contact  from '../Home/Contact'
import Login  from '../Login/View'
import Register  from '../Register/ChallengeContact/View'
import RegisterVerify  from '../Register/ConfirmSecret/View'
import RegisterRedeem  from '../Register/CreateLogin/View'
import BadRequest  from '../Errors/400'
import NotFound  from '../Errors/404'

export default class App extends Component {
  static propTypes = {
    store: PropTypes.object.isRequired,
    history: PropTypes.object.isRequired
  };

  render() {
    const { store, history } = this.props
    return(
      <Provider store={store}>
        <Router history={history}>
          <Route path="/" component={Layout}>
            <IndexRoute component={Home} />
            <Route path="about" component={About} />
            <Route path="contact" component={Contact} />
            <Route path="login" component={Login} />
            <Route path="register" component={Register} />
            <Route path="register/:correlationId" component={RegisterVerify} />
            <Route path="register/:correlationId/redeem" component={RegisterRedeem} />
            <Route path="errors/400" component={BadRequest} />
            <Route path="*" component={NotFound} />
          </Route>
        </Router>
      </Provider>
    )
  }
}
