import onSubmit from './actions'
import Helmet from 'react-helmet'
import Form from './Form'

export default ({ params, location: { query, }, }) => (
  <div>
    <Helmet title="Register" />
    <h2>Register.</h2>
    <h4>Create a new account.</h4>
    <hr />
    <Form onSubmit={onSubmit} initialValues={{ ...params, ...query }} />
  </div>
)
