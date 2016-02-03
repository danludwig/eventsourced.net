import onSubmit from './actions'
import Helmet from 'react-helmet'
import Form from './Form'

const View = ({ params, location: { query, }, }) => (
  <div>
    <Helmet title="Verify" />
    <h2>Verify.</h2>
    <h4>Confirm your contact information.</h4>
    <hr />
    <Form onSubmit={onSubmit} initialValues={{ ...params, ...query }} />
  </div>
)

View.propTypes = {
  params: React.PropTypes.shape({
    correlationId: React.PropTypes.string.isRequired,
  }).isRequired,
  location: React.PropTypes.shape({
    query: React.PropTypes.shape({
      returnUrl: React.PropTypes.string,
    }),
  }),
}

export default View
