import Helmet from 'react-helmet'

export default () => (
  <div>
    <Helmet title="Contact" />
    <h2>Contact.</h2>
    <h3></h3>
    <address>
      Not the Microsoft Way<br />
      Redmond, WA 98052-6399<br />
      <span>
        <abbr title="Phone">P:</abbr>
        {' '}
        <span>425.555.0100</span>
      </span>
    </address>
    <address>
      <strong>Support:</strong> <a href="mailto:Support@example.com">Support@example.com</a><br />
      <strong>Marketing:</strong> <a href="mailto:Marketing@example.com">Marketing@example.com</a>
    </address>
  </div>
)
