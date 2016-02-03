import { connect } from 'react-redux'
import onSubmit from './actions'
import Helmet from 'react-helmet'
import Form from './Form'

const View = ({ viewData, params, location: { query, }, }) => (
  <div>
    <Helmet title="Create login" />
    <h2>Create login.</h2>
    <h4>Choose a username and password.</h4>
    <hr />
    <Form onSubmit={onSubmit}
          initialValues={{ ...params, ...query }}
          viewData={viewData}
    />
  </div>
)

View.propTypes = {
  viewData: React.PropTypes.shape({
    purpose: React.PropTypes.string.isRequired,
    contactValue: React.PropTypes.string.isRequired,
  }).isRequired,
  params: React.PropTypes.shape({
    correlationId: React.PropTypes.string.isRequired,
  }).isRequired,
  location: React.PropTypes.shape({
    query: React.PropTypes.shape({
      returnUrl: React.PropTypes.string,
    }),
  }),
}

const select = state => ({
  viewData: state.app.register.createLogin.viewData,
})
export default connect(select)(View)
