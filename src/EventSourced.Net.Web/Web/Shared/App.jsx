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

const App = ({ store, history }) => (
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

App.propTypes = {
  store: React.PropTypes.object.isRequired,
  history: React.PropTypes.object.isRequired,
}

export default App
